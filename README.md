# Arduino Radio

An attempt to simulate functionality for an old radio using Arduino Nano and Unity. Essentially, an illusion.

| Functionality  | Pin (Arduino Nano) | Function Called                                     |
|----------------|--------------------|-----------------------------------------------------|
| Play           | 8                  | `Play(AudioSource audio)`                           |
| Pause / Resume | 2                  | `Pause(AudioSource audio); Play(AudioSource audio)` |
| Fast Forward   | 10                 | `FastForward(float howMuch); FastBackwards()`       |
| Rewind         | 4                  | `Rewind(bool t);`                                   |
