# Endless Runner with Arduino & Radio Control

[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)   [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


An attempt to simulate functionality for an old radio using Arduino Nano and Unity. Essentially, an illusion.

| Functionality  | Pin (Arduino Nano) | Function Called                                     |
|----------------|--------------------|-----------------------------------------------------|
| Play           | 8                  | `Play(AudioSource audio)`                           |
| Pause / Resume | 2                  | `Pause(AudioSource audio); Play(AudioSource audio)` |
| Fast Forward   | 10                 | `FastForward(float howMuch); FastBackwards()`       |
| Rewind         | 4                  | `Rewind(bool t);`                                   |

---

This project transforms a standard Unity endless runner game into an interactive experience controlled through an Arduino setup, using a radio to simulate physical control. Our goal was to give the illusion that the game is running mechanically, though it's entirely powered by digital modifications in Unity, Arduino, and serial communication.

## **Screenshots**

![image](https://github.com/user-attachments/assets/ab94571d-c51e-4482-a788-656bb05f7863)

## **Features**

The radio dials and buttons, connected to an Arduino, send signals to Unity through serial communication. This setup allows:
- **Play**: Start running the game.
- **Pause**: Freeze everything in place.
- **Fast Forward**: Double the speed of the runner and environment.
- **Rewind**: Make the character run backward.
  
The radio acts as the interface, making it look as if the game is being controlled by analog devices. But behind the scenes, Unity and Arduino are managing all the logic.

## **Process**

1. **Radio to Arduino**: We connected the radio to the Arduino to act as the control interface. The dials and buttons trigger specific serial messages when adjusted or pressed. We use magnets and unipolar hall effect sensors to simulate button pressing functionality.
2. **Arduino to Unity**: The Arduino sends these messages to Unity, which interprets them and adjusts the game accordingly (play, pause, fast forward, rewind).
3. **Audio Integration**: The audio output is routed through the radio’s speakers, making it sound like the radio is generating the game’s music.

## **Setup Instructions**

### Custom Build

#### **1. Clone the Repository**
```bash
git clone https://github.com/Evil-Kitten-Corp/ArduinoRadio.git
cd ArduinoRadio
```

#### **2. Unity Project**

- Open the project in Unity (version 2022.3.10f or higher).
- Connect your Arduino to your PC (set to COM3 or update the COM port in `ArduinoController.cs`).
- Ensure the radio is properly connected via the audio jack to act as the control and sound source.

#### **3. Arduino Setup**

- Load the Arduino sketch that listens for inputs from the radio and sends corresponding serial commands to Unity.
- Adjust the radio dials to control the game (start, stop, fast forward, rewind).

### Released Build

- Connect your Arduino to your PC (make **sure** it's set to COM3).
- Ensure the radio is properly connected via the audio jack to act as the control and sound source.
- Load and upload the Arduino sketch to the Arduino.

## Dependencies

- Unity (2022.3.10f version or higher)
- Arduino IDE for microcontroller setup
- SerialPort communication libraries

## License

This project is licensed under the MIT License. See the [LICENSE.md](LICENSE.md) file for more details.
