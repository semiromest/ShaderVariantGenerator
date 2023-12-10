#Shader Variant Generator

Overview

The Shader Variant Generator is a Unity Editor tool that automates the generation of material variants based on a target material. It allows you to create multiple material instances with randomized property values, supporting features such as HDR variations and texture assignments.

Getting Started

Installation

Open your Unity project.

Copy the ShaderVariantGenerator.cs script into your project's Editor folder.

The tool can be accessed from the Unity Editor under Tools > Shader Variant Generator.

Usage

Open the Shader Variant Generator window by navigating to Tools > Shader Variant Generator.

Drag and drop the target material onto the designated field.

Set the desired parameters:

Min-Max Range: Specify the range for random property values.

Variant Count: Determine the number of material variants to generate.

HDR Variants: Enable HDR variations for color properties.

Texture Variants: Enable texture assignments for TexEnv properties.

If Texture Variants is enabled, provide the path to a folder containing texture files.

Click the "Generate Variants" button to create the material variants.

Notes

Material variants are saved in the Assets/Variants directory.

If the specified folder for textures is empty, a warning will be displayed.
