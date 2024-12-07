package com.dominik.plasku.finalprojectapplication

import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.os.Bundle
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class SensorActivity : AppCompatActivity(), SensorEventListener {

    private lateinit var sensorManager: SensorManager
    private var temperatureSensor: Sensor? = null
    private lateinit var textViewX: TextView
    private lateinit var textViewY: TextView
    private lateinit var textViewZ: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_sensor)

        textViewX = findViewById(R.id.textViewX)
        textViewY = findViewById(R.id.textViewY)
        textViewZ = findViewById(R.id.textViewZ)

        sensorManager = getSystemService(SENSOR_SERVICE) as SensorManager
        temperatureSensor = sensorManager.getDefaultSensor(Sensor.TYPE_AMBIENT_TEMPERATURE)

        sensorManager.registerListener(this, temperatureSensor, SensorManager.SENSOR_DELAY_UI)
    }

    override fun onSensorChanged(event: SensorEvent?) {
        if (event == null) return

        if (event.sensor.type == Sensor.TYPE_AMBIENT_TEMPERATURE) {
            val temperature = event.values[0]
            textViewX.text = "X: Temperature: $temperature °C"
            textViewY.text = "No Y data for this sensor"
            textViewZ.text = "No Z data for this sensor"
        }
    }

    override fun onAccuracyChanged(sensor: Sensor?, accuracy: Int) {

    }

    override fun onPause() {
        super.onPause()
        sensorManager.unregisterListener(this)
    }
}
