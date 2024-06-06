using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;
using System;

namespace RunJoyStickOnLocalMachine
{
    class Joystick
    {
          #region param

        private Device joystickDevice;
        private JoystickState state;
        public int Lx; // X-axis movement
        public int Ly; //Y-axis movement
        public int Rx;
        public int Ry;
        public int[] Dpad;
        private IntPtr hWnd;
        public bool[] buttons;
        private string systemJoysticks;

        #endregion

        public Joystick(IntPtr window_handle)
        {
            hWnd = window_handle;
            Lx = -1;
        }

        public string FindJoysticks()
        {
            systemJoysticks = null;

            try
            {
                // Find all the GameControl devices that are attached.
                DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);

                // check that we have at least one device.
                if (gameControllerList.Count > 0)
                {
                    foreach (DeviceInstance deviceInstance in gameControllerList)
                    {
                        // create a device from this controller so we can retrieve info.
                        joystickDevice = new Device(deviceInstance.InstanceGuid);
                        joystickDevice.SetCooperativeLevel(hWnd, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);

                        systemJoysticks = joystickDevice.DeviceInformation.InstanceName;

                        break;
                    }
                }
            }
            catch
            {
                return null;
            }

            return systemJoysticks;
        }

        public bool AcquireJoystick(string name)
        {
            try
            {
                DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
                int i = 0;
                bool found = false;
                
                foreach (DeviceInstance deviceInstance in gameControllerList)
                {
                    if (deviceInstance.InstanceName == name)
                    {
                        found = true;
                        joystickDevice = new Device(deviceInstance.InstanceGuid);
                        joystickDevice.SetCooperativeLevel(hWnd, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                        break;
                    }
                    i++;
                }

                if (!found)
                    return false;

                joystickDevice.SetDataFormat(DeviceDataFormat.Joystick);

                joystickDevice.Acquire();

                UpdateStatus();
            }
            catch (Exception err)
            {
                return false;
            }

            return true;
        }

        public void ReleaseJoystick()
        {
            joystickDevice.Unacquire();
        }

        public void UpdateStatus()
        {
            Poll();

            Lx = state.X;
            Ly = state.Y;
            Rx = state.Z;
            Ry = state.Rz;
            Dpad = state.GetPointOfView();

            byte[] jsButtons = state.GetButtons();
            buttons = new bool[jsButtons.Length];

            int i = 0;
            foreach (byte button in jsButtons)
            {
                buttons[i] = button >= 128;
                i++;
            }

        }

        private void Poll()
        {
            try
            {
                // poll the joystick
                joystickDevice.Poll();
                // update the joystick state field
                state = joystickDevice.CurrentJoystickState;
            }
            catch
            {
                throw (null);
            }
        }
    }
    }
