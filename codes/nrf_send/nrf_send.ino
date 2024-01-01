#include <nRF24L01.h>
#include <printf.h>
#include <RF24.h>
#include <RF24_config.h>
#include <SPI.h>  
#include "RF24.h"
byte addresses[][6] = {"0"};
RF24 myRadio (7, 8); //Initializes the RF24 object with the specified Chip Enable (CE) and Chip Select Not (CSN) pin numbers.
int xPin = A0;
int yPin = A1;
int buttonPin = 4;


struct package
{
  int id=1;
  float temperature = 18.3;
  char  text[100] = "Text to be transmitted";
};


typedef struct package Package;
Package data;


void setup()
{
  pinMode(buttonPin, INPUT_PULLUP);
  Serial.begin(115200);
  delay(1000);
  myRadio.begin();  
  myRadio.setChannel(115); 
  myRadio.setPALevel(RF24_PA_MAX);
  myRadio.setDataRate( RF24_250KBPS ) ; 
  myRadio.openWritingPipe( addresses[0]);
  
  delay(1000);
}
void loop()
{
  int xVal = analogRead(xPin);
  int yVal = analogRead(yPin);
  int buttonVal = digitalRead(buttonPin);

  Serial.print("X = ");
  Serial.print(xVal);

  Serial.print("    Y = ");
  Serial.print(yVal);

  Serial.print("    Button = ");
  Serial.println(buttonVal);

  delay(100);
  myRadio.write(&data, sizeof(data)); 

  Serial.print("\nPackage:");
  Serial.print(data.id);
  Serial.print("\n");
  Serial.println(data.temperature);
  Serial.println(data.text);
  data.id = data.id + 1;
  data.temperature = data.temperature+0.1;
  delay(1000);

}
