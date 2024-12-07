package com.dominik.plasku.finalprojectapplication

import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        val textView1: TextView = findViewById(R.id.textView1)
        val editText1: EditText = findViewById(R.id.editText1)
        val editText2: EditText = findViewById(R.id.editText2)
        val button1: Button = findViewById(R.id.button1)
        val button2: Button = findViewById(R.id.button2)

        button1.setOnClickListener {
            val intent = Intent(this, SecondActivity::class.java)
            intent.putExtra("TEXT1", editText1.text.toString())
            intent.putExtra("TEXT2", editText2.text.toString())
            startActivity(intent)
        }

        button2.setOnClickListener {
            val phoneIntent = Intent(Intent.ACTION_DIAL)
            phoneIntent.data = Uri.parse("tel:1111111111")
            startActivity(phoneIntent)
        }
    }
}