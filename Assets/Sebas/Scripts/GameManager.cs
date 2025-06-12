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
    public GameObject startPanel;
    private AudioSource audioSource;

    [SerializeField] private AudioClip audioGameOver;

    [Header("UI")]
    public TextMeshProUGUI vidaText;
    public TextMeshProUGUI puntajeText;

    [Header("Valores del juego")]
    public int vidaInicial = 3;
    private int vidaActual;
    private int puntajeActual;

    private void Awake ()
    {
        // Singleton: solo un GameManager en escena
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        vidaActual = vidaInicial;
        puntajeActual = 0;
        ActualizarUI();
    }

    public void RestarVida ( int cantidad )
    {
        vidaActual -= cantidad;

        Debug.Log("Jugador recibió daño. Nivel actual: " + vidaActual);
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            audioSource.PlayOneShot(audioGameOver);
            GameOver();
        }
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

    private void GameOver ()
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(EsperarGameOver());
        Time.timeScale = 0f; // Asegurarse de que el tiempo esté corriendo
        Debug.Log("Game Over");
        // Aquí puedes poner pantalla de game over, reinicio, etc.
    }

    IEnumerator EsperarGameOver ()
    {
        yield return new WaitForSeconds(2);
    }

    IEnumerator EsperarYReiniciarJuego ()
    {
        yield return new WaitForSeconds(2);
        ReiniciarJuego();
    }

    public void IniciarJuego ()
    {
        startPanel.SetActive(false);
    }

    public void ReiniciarJuego ()
    {
        // Reiniciar el juego, por ejemplo, recargando la escena actual
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté corriendo
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void Salir ()
    {
        Application.Quit();
    }


}
