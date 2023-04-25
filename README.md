<img src="Design/banner.jpg" style="margin-bottom:10px" />

# Juego

Open-source, Meadow-powered, multigame handheld console with DPads, speakers and a colored display.

## Contents
* [Purchasing or Building](#purchasing-or-building)
* [Getting Started](#getting-started)
* [Hardware Specifications](#hardware-specifications)
* [Pinout Diagram](#pinout-diagram)
  * [Juego v2.c](#project-lab-v1e)
* [Additional Samples](#additional-samples)

## Purchasing or Building

<table width="100%">
    <tr>
        <td>
            <img src="Design/juego-store.jpg" />
        </td>
        <td>
            <img src="Design/juego-hackkit.jpg" /> 
        </td>
    </tr>
    <tr>
        <td>
            You can get a Juego board from the <a href="https://store.wildernesslabs.co/collections/frontpage/products/project-lab-board">Wilderness Labs store</a>.
        </td>
        <td> 
            You can also build a simpler Juego using a monocolor display and push buttons.
        </td>
    </tr>
</table>

## Getting Started

To make using the hardware even simpler, we've created a Nuget package that instantiates and encapsulates the onboard hardware into a `Juego` class.

1. Add the ProjectLab Nuget package your project: 
    - `dotnet add package Meadow.Juego`, or
    - [Meadow.Juego Nuget Package](https://www.nuget.org/packages/Meadow.Juego)
    - [Explore in Fuget.org](https://www.fuget.org/packages/Meadow.Juego/0.1.0/lib/netstandard2.1/Juego.dll/Meadow.Devices/Juego)

## Hardware Specifications

<img src="Design/juego-specs.jpg" style="margin-top:10px;margin-bottom:10px" />

<table>
    <tr>
        <td><strong>ILI9341</strong> - 240x320 RGB LED display</td>
    </tr>
    <tr>
        <td><strong>Magnetic Audio Transducers</strong> - A pair of High quality piezo speakers</td>
    </tr>
    <tr>
        <td><strong>DPads</strong> - Directional Pads on either side of the display.</td>
    </tr>
    <tr>
        <td><strong>Select/Start Buttons</strong> - Classical Select and Start buttons</td>
    </tr>
    <tr>
        <td><strong>USB-C port</strong> - Port used to build/deploy apps to Juego</td>
    </tr>
    <tr>
        <td><strong>Rechargable Battery Slot</strong> - To play on the go</td>
    </tr>
    <tr>
        <td><strong>On/Off switch</strong> - Used to turn the Juego on and off</td>
    </tr>
</table>

You can find the schematics and other design files in the [Hardware folder](Source/Hardware).

## Building locally

When using the develop branch, you'll need to clone additional Meadow repos:

- [Meadow.Core](https://github.com/WildernessLabs/Meadow.Core)
- [Meadow.Units](https://github.com/WildernessLabs/Meadow.Units)
- [Meadow.Contracts](https://github.com/WildernessLabs/Meadow.Contracts)
- [Meadow.Foundation](https://github.com/WildernessLabs/Meadow.Foundation)
- [Meadow.Logging](https://github.com/WildernessLabs/Meadow.Logging)

Be sure to clone all these repos including `Juego` at the same folder level and set all repos to the `develop` branch.

Also, make sure you are running the latest version of Meadow OS on your Juego v2 board.

## Testing Juego

To validate your Juego hardware, connect your Juego, and deploy the Juego_Demo application.

## Juego Prototype

This was an early hardware project to create a Meadow handheld multi-game project designed to work with 128x64 or 320x240 single color displays (SSD1306 or SSD1309)

Includes five games:

- FrogIt
- Pong
- Span4 (2-player match 4 game)
- Snake
- Tetraminos (inspired by Tetris)

!["Image of Juego Meadow prototype hardware"](Design/juego-ping-pong.jpg)

## Fritzing Diagrams of Juego using a Meadow Dev Kit

<table width="100%">
    <tr>
        <td>
            <img src="Design/juego_spi_fritzing.png" />
        </td>
        <td>
            <img src="Design/juego_i2c_fritzing.png" /> 
        </td>
    </tr>
    <tr>
        <td>
            Juego using a SSD1309 Display connected via SPI and Push Buttons.
        </td>
        <td> 
            Juego using a SSD1309 Display connected via I2C and Push Buttons.
        </td>
    </tr>
</table>

