# CoverControl

**CoverControl** is a CLI application that allows for automated control for a remote telescope.

Application is operated in a CLI form on an interactive basis with main menu and other submenus being provided at any time. It aims to provide a one-stop environment for operating the telescope apparatus, and it enables executing custom workloads at custom times.

Let us go over each option of the main menu:

![](application.png)

## 0) Relay Control

This enables executing an external application that enables the relay module. Author of this application has not been able to find any drivers of libraries they could use programmatically within the application, so they have resorted to providing a path to an external application that the clients have access to. Upon first launch, user is asked to provide a path to this external application, and future launches will launch the external application automatically.

## 1) Control servo motors

Enables control of specific servo motors on individual channels. It operates on a min/max scale that can be manually customized (this depends on the hardware in use). Furthermore, user can specify the servo channel width of the relay module in use, as well as with speed and acceleration parameters (see below).

## 2) Execute XML sequence

Enables execution of an arbitrary sequence provided within an XML file. User provides a path to the XML file, so that the instructions are loaded in and executed accordingly. User can specify frames for Open and Close sequences, where each frame has a name, execution length in milliseconds, and states for all of the channels. Acceleration and speed are subject to global settings (see below)

## 3) Create XML sequence

User can create custom sequences that they can later execute. Each sequence is a succession of frames whose execution order is subject to order of user input. Each frame has a defined length in milliseconds and the states that all of the relay channels should follow. Created XML files are compatible with [Pololu Maestro Control Center](https://www.pololu.com/docs/0J40/4).

## 4) Schedule

User can specify a workload to be executed upon sunset or sunrise (which are dynamically calculated based on computer's time), or they can schedule to run certain workloads at a custom time of the day they have specified. This schedule runs intelligently, meaning it automatically picks up at the nearest instance when the time will occur, either on the same day or the next day compared to computer time.

## 5) Global Settings

This contains all the values used throughout the application in XML files, but they also allow customization of servo acceleration and speed, as well as specifying custom ranges for the hardware in question. User can also add custom properties or reset all of the values to their defaults. Lastly, they can use it to customize the longitude and latitude of their telescope, which are both used for calculating sunrise and sunset times dynamically.

![](global.png)
