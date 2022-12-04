// 
// 
// 

#include "PINOUT.h"

void PINOUT::init()
{
    pinMode(pin, OUTPUT);
    off();
}

PINOUT::PINOUT(uint8_t pin, bool _state = false)
{
    this->pin = pin;
    if (_state) {
        this->reverse = _state;
    }
    init();
}

void PINOUT::on()
{
    if (this->reverse) {
        digitalWrite(pin, HIGH);
    }
    else {
        digitalWrite(pin, LOW);
    }
    statePin = true;
}
void PINOUT::off()
{
    if (this->reverse) {
        digitalWrite(pin, LOW);
    }
    else {
        digitalWrite(pin, HIGH);
    }
    statePin = false;
}