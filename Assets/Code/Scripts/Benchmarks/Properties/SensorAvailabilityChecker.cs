using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorAvailabilityChecker : MonoBehaviour
{
    [SerializeField] private bool accelerometerAvailable;
    [SerializeField] private bool gyroscopeAvailable;
    [SerializeField] private bool compassAvailable;
    [SerializeField] private bool lightSensorAvailable;
    [SerializeField] private bool proximitySensorAvailable;
    [SerializeField] private bool magneticFieldSensorAvailable;
    [SerializeField] private bool gravitySensorAvailable;
    [SerializeField] private bool linearAccelerationSensorAvailable;
    [SerializeField] private bool pressureSensorAvailable;
    [SerializeField] private bool humiditySensorAvailable;
    [SerializeField] private bool temperatureSensorAvailable;
    [SerializeField] private bool stepCounterSensorAvailable;
    [SerializeField] private bool AttitudeSensorAvailable;

    private void Start() { 

        CheckAccelerometerAvailability();
        CheckGyroscopeAvailability();
        CheckCompassAvailability();
        CheckLightSensorAvailability();
        CheckProximitySensorAvailability();
        CheckMagneticFieldSensorAvailability();
        CheckGravitySensorAvailability();
        CheckLinearAccelerationSensorAvailability();
        CheckPressureSensorAvailability();
        CheckHumiditySensorAvailability();
        CheckTemperatureSensorAvailability();
        CheckStepCounterSensorAvailability();
        CheckAttitudeSensorAvailability();
    }

    private void CheckSensorAvailability(string sensorName, bool isAvailable)
    {
        Debug.Log(sensorName + " Sensor: " + (isAvailable ? "Available" : "Not Available"));
    }

    private void CheckAccelerometerAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is Accelerometer);
        accelerometerAvailable = sensor != null ;
    }

    private void CheckGyroscopeAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is UnityEngine.InputSystem.Gyroscope);
        gyroscopeAvailable = sensor != null;
    }

    private void CheckAttitudeSensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is AttitudeSensor);
        AttitudeSensorAvailable = sensor != null;
    }

    private void CheckCompassAvailability()
    {
        compassAvailable = Input.compass.enabled;
    }

    private void CheckLightSensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is LightSensor);
        lightSensorAvailable = sensor != null; 

    }

    private void CheckProximitySensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is ProximitySensor);
        proximitySensorAvailable = sensor != null;
    }

    private void CheckMagneticFieldSensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is MagneticFieldSensor);
        magneticFieldSensorAvailable = sensor != null;
    }

    private void CheckGravitySensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is GravitySensor);
        gravitySensorAvailable = sensor != null;
    }

    private void CheckLinearAccelerationSensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is LinearAccelerationSensor);
        linearAccelerationSensorAvailable = sensor != null;
    }

    private void CheckPressureSensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is PressureSensor);
        pressureSensorAvailable = sensor != null;
    }

    private void CheckHumiditySensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is HumiditySensor);
        humiditySensorAvailable = sensor != null;
    }

    private void CheckTemperatureSensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is AmbientTemperatureSensor);
        temperatureSensorAvailable = sensor != null;
    }

    private void CheckStepCounterSensorAvailability()
    {
        InputDevice sensor = InputSystem.devices.FirstOrDefault(device => device is StepCounter);
        stepCounterSensorAvailable = sensor != null;
    }
}


