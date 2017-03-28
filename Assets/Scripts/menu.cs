using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {
	public Canvas quit_menu;
	public Canvas how_to_play;

	// Use this for initialization
	void Start () {
		quit_menu = quit_menu.GetComponent<Canvas> ();
		how_to_play = how_to_play.GetComponent<Canvas> ();

		quit_menu.enabled = false;
		how_to_play.enabled = false;
	}
	
	public void how_to_play_play()
	{
		how_to_play.enabled = true;

	}
	public void exit_how_to()
	{
		how_to_play.enabled = false;
	}

	public void load_level()
	{
		SceneManager.LoadScene ("Level1");
	}


	public void exit_game()
	{
		Application.Quit ();
	}

	public void exit_press()
	{
		quit_menu.enabled = true;
	}
	public void no_exit()
	{
		quit_menu.enabled = false;

	}


}
