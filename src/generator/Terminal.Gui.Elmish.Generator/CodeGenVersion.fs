module Terminal.Gui.Elmish.Generator.CodeGenVersion

open System.IO
open System.Text.Json

type CodeGenVersion = {
  GitHash: string
  PaketLockHash: string
}

[<Literal>]
let JsonFileName = "CodeGenVersion.json"

let prevCodeGenVersion =
  if not (File.Exists(JsonFileName)) then
    {
      GitHash = ""
      PaketLockHash = ""
    }
  else
    JsonSerializer.Deserialize<CodeGenVersion>(File.ReadAllText(JsonFileName))

let currentGitHash =
  let proc = System.Diagnostics.Process.Start(
    System.Diagnostics.ProcessStartInfo(
      FileName = "git",
      Arguments = "rev-parse HEAD",
      RedirectStandardOutput = true,
      UseShellExecute = false
    )
  )
  proc.WaitForExit()
  proc.StandardOutput.ReadToEnd().Trim()

let gitRoot =
  let proc = System.Diagnostics.Process.Start(
    System.Diagnostics.ProcessStartInfo(
      FileName = "git",
      Arguments = "rev-parse --show-toplevel",
      RedirectStandardOutput = true,
      UseShellExecute = false
    )
  )
  proc.WaitForExit()
  proc.StandardOutput.ReadToEnd().Trim()

let currentPaketLockHash =
  use sha256 = System.Security.Cryptography.SHA256.Create()
  use stream = File.OpenRead(Path.Combine(gitRoot, "paket.lock"))
  let hash = sha256.ComputeHash(stream)
  System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant()

let newGenNeeded =
  prevCodeGenVersion.GitHash <> currentGitHash ||
  prevCodeGenVersion.PaketLockHash <> currentPaketLockHash

let saveCurrentCodeGenVersion () =
  let info = {
    GitHash = currentGitHash
    PaketLockHash = currentPaketLockHash
  }
  let json = JsonSerializer.Serialize(info, JsonSerializerOptions(WriteIndented = true))
  File.WriteAllText(JsonFileName, json)

