package com.dominik.plasku.finalprojectapplication

import android.content.Intent
import android.os.Bundle
import android.view.KeyEvent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.inputmethod.EditorInfo
import android.widget.Button
import android.widget.EditText
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import android.widget.TextView

class SecondActivity : AppCompatActivity() {

    private lateinit var adapter: VerseAdapter

    private val verses = mutableListOf(
        "Dashing through the snow",
        "In a one-horse open sleigh",
        "O'er the fields we go",
        "Laughing all the way.",
        "The bells on bob-tail ring",
        "They make our spirits bright",
        "What fun it is to ride and sing",
        "A sleighing song tonight.",
        "Jingle bells, jingle bells",
        "Jingle all the way,",
        "Oh, what fun it is to ride",
        "In a one-horse open sleigh!",
        "Jingle bells, jingle bells",
        "Jingle all the way,",
        "Oh, what fun it is to ride",
        "In a one-horse open sleigh!"
    )

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_second)

        val text1 = intent.getStringExtra("TEXT1")
        val text2 = intent.getStringExtra("TEXT2")

        val editText4 = findViewById<EditText>(R.id.editText4)
        editText4.setText(text2)

        editText4.setOnEditorActionListener { _, actionId, event ->
            if (actionId == EditorInfo.IME_ACTION_DONE || (event?.keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_DOWN)) {
                val newText = editText4.text.toString()
                if (newText.isNotBlank()) {
                    verses.add(newText)
                    adapter.notifyItemInserted(verses.size - 1)
                    editText4.text.clear()
                }
                true
            } else {
                false
            }
        }

        val textView2 = findViewById<TextView>(R.id.textView2)
        textView2.text = text1

        val recyclerView = findViewById<RecyclerView>(R.id.recyclerView)
        recyclerView.layoutManager = LinearLayoutManager(this)
        adapter = VerseAdapter(verses)
        recyclerView.adapter = adapter

        val button3 = findViewById<Button>(R.id.button3)
        button3.setOnClickListener {
            val intent = Intent(this, SensorActivity::class.java)
            startActivity(intent)
        }
    }
    class VerseAdapter(private val verses: List<String>) : RecyclerView.Adapter<VerseAdapter.VerseViewHolder>() {

        class VerseViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
            val textView: TextView = itemView.findViewById(android.R.id.text1)
        }

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): VerseViewHolder {
            val view = LayoutInflater.from(parent.context).inflate(android.R.layout.simple_list_item_1, parent, false)
            return VerseViewHolder(view)
        }

        override fun onBindViewHolder(holder: VerseViewHolder, position: Int) {
            holder.textView.text = verses[position]
        }

        override fun getItemCount() = verses.size
    }
}