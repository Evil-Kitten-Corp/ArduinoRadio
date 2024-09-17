# Arduino Radio

An attempt to simulate functionality for an old radio using Arduino Nano and Unity. Essentially, an illusion.

| Functionality  | Pin (Arduino Nano) | Function Called                                     |
|----------------|--------------------|-----------------------------------------------------|
| Play           | 2                  | `Play(AudioSource audio)`                           |
| Pause / Resume | 6                  | `Pause(AudioSource audio); Play(AudioSource audio)` |
| Fast Forward   | 8                  | `FastForward(float howMuch); FastBackwards()`       |
| Rewind         | 11                 | `Rewind(bool t);`                                   |
