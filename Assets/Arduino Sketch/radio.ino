//digital sensor setup
const int hallDigitalPinRickRoll = 6;  // rick roll pin / pause
const int hallDigitalFastForward = 8; // fast forward pin
const int hallDigitalRewind = 11; // rewind pin
const int hallDigitalPlay = 2; // play pin

int rewindTrack;
int fastForward;
int playBttn;
int hallDigiRickRoll;

bool isPlayOn = false;

bool isRickRollPlaying = false;
bool isWhatIsLovePlaying = false;
bool fastForwarding = false;
bool rewinding = false;

void setup() {
  pinMode(hallDigitalPinRickRoll, INPUT_PULLUP);
  pinMode(hallDigitalFastForward, INPUT_PULLUP);
  pinMode(hallDigitalPlay, INPUT);
  pinMode(hallDigitalRewind, INPUT_PULLUP);
  Serial.begin(9600);
}

void loop() 
{
  if (isPlayOn = true) 
  {
    hallDigiRickRoll = digitalRead(hallDigitalPinRickRoll);
    fastForward = digitalRead(hallDigitalFastForward);
    rewindTrack = digitalRead(hallDigitalRewind);

    // Rick roll song
    if (hallDigiRickRoll == LOW && !isRickRollPlaying) {
      Serial.println("Play");
      isRickRollPlaying = true;
      isWhatIsLovePlaying = false;
    } else if (hallDigiRickRoll == HIGH && isRickRollPlaying) {
      Serial.println("Pause");
      isRickRollPlaying = false;
    }

    // Fast forward song
    if (fastForward == LOW && !fastForwarding) {
      Serial.println("Fastf");
      fastForwarding = true;
    } else if (fastForward == HIGH && fastForwarding) {
      Serial.println("Backf");
      fastForwarding = false;
    }

    // Rewind song
    if (rewindTrack == LOW && !rewinding) {
      Serial.println("Rewind ON");
      rewinding = true;
    } else if (rewindTrack == HIGH && rewinding) {
      Serial.println("Rewind OFF");
      rewinding = false;
    }
  }
  else 
  {
    playBttn = digitalRead(hallDigitalPlay);

    if (playBttn == LOW) 
    {
      Serial.println("Play");
      isPlayOn = true;
    }
  }

  delay(100); // Add a small delay to avoid flooding the serial port
}
