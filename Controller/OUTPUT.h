// OUTPUT.h

#ifndef _OUTPUT_h
#define _OUTPUT_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

class OUTPUT
{
 protected:
	 uint8_t pin;
	 bool stateAutoOff = false;
	 bool statePin;
	 unsigned long lastDebounceTime;
	 int debounceDelay = 170;
	 bool reverse = false;
 public:
	 PINOUT(uint8_t pin, bool _state = false);
	 void init();
	 void on(bool autoOff = false);
	 void off();
	 void setDelay(int _Delay);
	 void setReverse(bool _state);
	 void update();
	 bool getState();
	 uint8_t getPin()
};


#endif

