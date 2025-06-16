using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject player;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject startPanel;
    private AudioSource audioSource;

    [SerializeField] private AudioClip audioGameOver;
    [SerializeField] private AudioClip audioDaño;
    [SerializeField] private AudioClip audioWin;

    [Header("UI")]
    public TextMeshProUGUI vidaText;
    public TextMeshProUGUI puntajeText;

    [Header("Prefabs de jugador")]
    public GameObject[] playerPrefabs;  // Aquí asignas los dos prefabs

    private GameObject currentPlayer;
    private int nivelActual = 1;

    [Header("Valores del jugador")]
    public int vidaInicial = 3;
    private int vidaActual;
    private int puntajeActual;

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentPlayer = GameObject.FindWithTag("Player");  // Si existe un player inicial en escena lo asigna
    }


    private void Start ()
    {
        startPanel.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        vidaActual = vidaInicial;
        puntajeActual = 0;
        ActualizarUI();
        Time.timeScale = 0f; // Asegurarse de que el tiempo esté corriendo
    }
    public void IniciarPartida ()
    {
        nivelActual = 1;

        if (currentPlayer == null)
        {
            Vector3 posicionInicial = new Vector3(-3, 1.76f, 0);
            InstanciarJugador(posicionInicial);
        }
    }


    public void SubirNivel ( Vector3 posicionActual )
    {
        nivelActual++;
        InstanciarJugador(posicionActual);
    }


    void InstanciarJugador ( Vector3 posicion )
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        currentPlayer = Instantiate(playerPrefabs[nivelActual - 1], posicion, Quaternion.identity);

        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        if (camera != null)
            camera.player = currentPlayer.transform;
    }


    public int GetNivelActual ()
    {
        return nivelActual;
    }


    public void RestarVida ( int cantidad )
    {
        vidaActual -= cantidad;

        audioSource.PlayOneShot(audioDaño);
        Debug.Log("Jugador recibió daño. Nivel actual: " + vidaActual);
        //nivel--;
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            audioSource.PlayOneShot(audioGameOver);
            GameOver();
        }
        ActualizarUI();
    }

    public void RestaurarVida ()
    {
        vidaActual ++;
        ActualizarUI();
    }


    public void SumarPuntaje ( int cantidad )
    {
        puntajeActual += cantidad;
        ActualizarUI();
    }

    private void ActualizarUI ()
    {
        vidaText.text = "Vida: " + vidaActual.ToString();
        puntajeText.text = "Puntaje: " + puntajeActual.ToString();
    }


    public void GameOver ()
    {
        audioSource.Pause();
        StartCoroutine(EsperarGameOver());
        audioSource.PlayOneShot(audioGameOver);

        Time.timeScale = 0f; // Asegurarse de que el tiempo esté corriendo
        Debug.Log("Game Over");
        // Aquí puedes poner pantalla de game over, reinicio, etc.
    }

    public void WinGame ()
    {

        audioSource.Pause();
        StartCoroutine(EsperarWin());
        audioSource.PlayOneShot(audioWin);

        Time.timeScale = 0f; // Asegurarse de que el tiempo esté corriendo
        Debug.Log("You Win");
        // Aquí puedes poner pantalla de game over, reinicio, etc.
    }

    public void PauseGame ()
    {
        audioSource.Pause();
        startPanel.gameObject.SetActive(true);

        Time.timeScale = 0f; // Asegurarse de que el tiempo esté corriendo
    }


    IEnumerator EsperarGameOver ()
    {
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(2);
    }

    IEnumerator EsperarWin ()
    {
        winPanel.SetActive(true);
        yield return new WaitForSeconds(2);
    }

    public void IniciarJuego ()
    {
        startPanel.SetActive(false);
        audioSource.Play(); // Reproducir música de fondo al iniciar el juego
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté corriendo  
    }

    public void ReiniciarJuego ()
    {
        PlayerController.nivel = 1;  // Reiniciamos el nivel estático
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


    public void Salir ()
    {
        Application.Quit();
    }


}
