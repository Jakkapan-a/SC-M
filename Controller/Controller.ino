/*
 Name:		Controller.ino
 Created:	12/4/2022 2:00:53 PM
 Author:	Jakkapan
*/
#include "PINOUT.h"
#include "BUTTON.h"

BUTTON button_1(8);
PINOUT r1(10);
PINOUT r2(11);

bool stringComplete = false; // whether the string is complete
String inputString = "";
bool state_1 = false;
void serialEvent() {
	while (Serial.available()) {
		// get the new byte:
		char inChar = (char)Serial.read();
		if (inChar == '#') {
			stringComplete = true; // Set state complete to true
			inputString.trim();
			break;
		}
		if (inChar == '>' || inChar == '<' || inChar == '\n' || inChar == '\r' || inChar == '\t' || inChar == ' ' || inChar == '?') {
			continue;
		}
		inputString += inChar;
	}
}
// the setup function runs once when you press reset or power the board
void setup() {
	Serial.begin(115200);
}

// the loop function runs over and over again until power down or reset
void loop() {

	if (button_1.isPressed() && state_1 ) {
		r1.off();
		r2.off();
   state_1 =false;
   Serial.println("Off");
	}
	if (stringComplete) {
		if (inputString == "OK") {
			r1.on();
			r2.on();
      state_1 = true;
      
		}
    Serial.println("Received");
    inputString ="";
		stringComplete = false;
  }
}
