# :file_folder: Test Files Generation Tool

:pushpin: Currently, the project is included to my summary repository of demo projects:

:link: [Demo Projects Workshop 2023+](https://github.com/dar920910/Demo-Projects-Workshop)

---

## :sound: About the Project History

This project is my attempt to implement a **flexible** tool to generate files by a custom template.

I created this software in 2021 to generate test media base environment (~ 200 000 files) for the Nexio AMP server when I've worked in the [Nexio MOS](https://imaginecommunications.com/product/nexio-amp/) commercial project as QA engineer.

Now I finished migration of the tool from .NET Core 3.1 to **.NET 6** target platform.
Also there were removed all mentions related to Nexio commercial product.

## :question: About the Repository Structure

This repository contains the following projects:

- **TestFilesGenerator.Library** - implements the .NET class library for the project
- **TestFilesGenerator.Testing** - implements NUnit-based unit tests for the project
- **TestFilesGenerator.App.CLI** - implements the console application for the project

---

## :beginner: Quick Start

1. Run the program from the command-line to create the configuration default template when the first launching.
2. Open the generated **config.xml** file from the appeared the **storage** folder in your favorite text editor.
3. Create your own description to generate custom file objects. See the "Reference" section below to get help.
4. Run the program after configuration creating is finished. Your generated files are in the **storage/out** directory.

## :green_book: Reference

Program's **config.xml** file supports the following parameters to create custom collections of file objects:

- **Name** - Name of the custom collection and its sub-folder into the **storage/out** directory.
- **SourceFileObject** - Path to the source template file. It's the "source.txt" by default.
- **CountOfFileObjects** - Count of file objects should be generated into the the custom collection.
- **HasRandomFileNames** - If its value is 'true' then file objects should be generated with random names, else not.
- **RandomNameLength** - Length of the base file name for a file object with a random name.

---

## :whale: Run via Docker

1. Build app's Docker image via **Execute-DockerBuild.ps1** script.
2. Run the app from a new container via **Execute-DockerRun.ps1** script.

## :wrench: Build Source Code

Use **.NET 6 SDK** to build this project from source code.

---

## :email: Contribute the Project

[You can contact me using any contacts from my profile page](https://github.com/dar920910#speech_balloon-how-can-you-contact-with-me-)
