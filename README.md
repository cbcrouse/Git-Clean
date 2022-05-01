# Overview

A dotnet core CLI tool that helps automate managing repository maintenance.

---

## Objective

Git.Clean is designed to automate maintenance tasks for git repositories by allowing developers to work with the tool locally. This tool does not support running in pipelines due to the interactive credentials. The destructive outcome of some commands and will likely require elevated access.

---

## Get Started

In order to install the tool, run the following command:

```bash
dotnet tool install --global Git.Clean
```

Next, run the command to show the Help information:

```bash
git-clean

Usage: git-clean [command] [options]

Options:
  -?|-h|--help           Show help information.

Commands:
  remove-stale-branches

Run 'git-clean [command] -?|-h|--help' for more information about a command.
```

---

## Available Commands

---

* **remove-stale-branches**

  This command will search through all the branches of the repository, ignoring any that match protected branch names (these can be provided in the `--ignore-branches` argument). Any branches that are older than provided `--stale-months` argument will be subject to removal. The branch matching is impossible to be perfect for every need, so review the output carefully before deciding to continue with any destructive actions.

  ```bash
  git-cleanup remove-stale-branches --help

  Usage: git-cleanup remove-stale-branches [options]

  Options:
    -g|--git-directory <GIT_DIRECTORY>      The directory containing the .git folder.
    -t|--remote-target <REMOTE_TARGET>      The git remote target. Defaults to 'origin'.
    -m|--stale-months <STALE_MONTHS>        The number of months without commits before a branch is considered stale. Defaults to 3.
    -i|--ignore-branches <IGNORE_BRANCHES>  The git remote branches to ignore. Defaults to [ "dev", "develop", "main",
                                            "master", "release", "HEAD" ]
    -?|-h|--help                            Show help information.
  ```

  **Example**

  ```bash
  git-cleanup remove-stale-branches -g "C:\Git\my-repo"
  ```

  **Example Output**

  ```base
  ...
  Excluding Branch: refs/remotes/origin/release/2.1
  Excluding Branch: refs/remotes/origin/release/3.1
  Excluding Branch: refs/remotes/origin/release/5.0
  Excluding Branch: refs/remotes/origin/release/6.0
  Excluding Branch: refs/remotes/origin/release/7.0-preview3
  Excluding Branch: refs/remotes/origin/release/7.0-preview4
  Excluding Branch: refs/remotes/origin/sebros/eol-main
  refs/remotes/origin/stevesa/e2e-test-spot-check
  refs/remotes/origin/t-mabuc/drag-and-drop
  refs/remotes/origin/t-mbuck/code-gen-component-parameters
  refs/remotes/origin/taparik/playwrightExperiment
  refs/remotes/origin/test-auth-templates
  refs/remotes/origin/updayte
  refs/remotes/origin/wtgodbe/HostingMu21
  refs/remotes/origin/wtgodbe/x6486
  Found 100 stale branches to delete. This action is destructive and cannot be reversed, do you wish to continue? [y/N]
  ```

---

## Contributing

This repository is automatically versioned by [Versioning.NET](https://github.com/cbcrouse/Versioning.NET). Please adhere to the [commit message standards](https://github.com/cbcrouse/Versioning.NET/blob/main/docs/commit_message_standards.md) required by the tool to ensure the semantic version is maintained correctly.
