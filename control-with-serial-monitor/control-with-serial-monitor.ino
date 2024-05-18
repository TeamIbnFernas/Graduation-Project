#include <SPI.h>
#include <nRF24L01.h>  //Remember to isntall this bibrry: http://www.electronoobs.com/engarduino_NRF24_lib.php
#include <RF24.h>
#include <Sharer.h>

const uint64_t my_radio_pipe = 0xE8E8F0F0E1LL;  //Remember that this code should be the same for the receiver

RF24 radio(9, 10);

// The sizeof this struct should not exceed 32 bytes
struct Data_to_be_sent {
  byte ch1;
  byte ch2;
  byte ch3;
  byte ch4;
  // byte ch5;
  // byte ch6;
};

//Create a variable with the structure above and name it sent_data
Data_to_be_sent sent_data;

void setup() {
  Sharer.init(115200);
  radio.begin();
  radio.setAutoAck(false);
  radio.setDataRate(RF24_250KBPS);
  radio.openWritingPipe(my_radio_pipe);

  
  // sent_data.ch1 = 0;
  // sent_data.ch2 = 0;
  // sent_data.ch3 = 0;
  // sent_data.ch4 = 0;
  Sharer_ShareVariable(byte, sent_data.ch1);
  Sharer_ShareVariable(byte, sent_data.ch2);
  Sharer_ShareVariable(byte, sent_data.ch3);
  Sharer_ShareVariable(byte, sent_data.ch4);
}

void loop() {
  Sharer.run();
  radio.write(&sent_data, sizeof(Data_to_be_sent));
}
