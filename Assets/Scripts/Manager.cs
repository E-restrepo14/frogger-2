using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    public GameObject tiempoText;
    public GameObject lifeHud;


    public static Manager instance;
    public bool isdead = false;
    public bool haswin = false;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject levelWinPanel;
    public GameObject[] obstacles;
    public float obstacleSpeed;
    public GameObject playerParent;
    public GameObject VictoryParticlesSystem;
    public float tiempoLimite = 10f;
    [SerializeField]
    private float m_tiempoLimite;
    public int vidas = 5;
    [SerializeField]
    private int m_vidas;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        if (instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        m_tiempoLimite = tiempoLimite;
        m_vidas = vidas;
        obstacles = GameObject.FindGameObjectsWithTag("obstacleTag");
    }
    void Update()
    {
        if (haswin == false & isdead == false )
        {
            m_tiempoLimite -= Time.deltaTime;
            if (m_tiempoLimite > 0)
            {

                tiempoText.GetComponent<Text>().text = ("tiempo limite: " + m_tiempoLimite.ToString("f0"));
            }
            else
            {
                isdead = true;
                StartCoroutine(Manager.instance.AnimarMuertePorTiempo());
            }
        }

        MoverObstaculos();
    }
    public void MoverObstaculos()
    {
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.transform.Translate(0, 0, obstacleSpeed * Time.deltaTime);

            if (obstacle.transform.localPosition.z >= 10)
            {
                obstacle.transform.localPosition = new Vector3(obstacle.transform.localPosition.x, obstacle.transform.localPosition.y, -10f);
                
            }
        }
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }

    public void Pausar()
    {
        Time.timeScale = 0f;
    }

    public void Despausar()
    {
        Time.timeScale = 1f;
    }

    void ModificarHudVidas()
    {
        for (int i = 0; i < lifeHud.transform.childCount; i++)
        {
            lifeHud.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int j = 0; j < m_vidas; j++)
        {
            lifeHud.transform.GetChild(j).gameObject.SetActive(true);
        }
    }

    public void VolverALinea1()
    {
        playerParent.transform.SetParent(null);
        playerParent.transform.position = new Vector3(0, 0.239f, 0);
        playerParent.transform.GetChild(0).GetComponent<Animator>().SetBool("hasCrashed", false);

        StartCoroutine(AnimarRespawn());
    }

    public IEnumerator AnimarMuertePorTiempo()
    {
        playerParent.transform.GetChild(1).GetComponent<Animator>().Play("timeover", 0, 0);
        playerParent.transform.GetChild(0).GetComponent<Animator>().Play("morir por tiempo", 0, 0);
        yield return new WaitForSeconds(1.5f);
        VolverALinea1();
    }
    IEnumerator AnimarRespawn()
    {
        playerParent.transform.GetChild(1).GetComponent<Animator>().Play("teleport circle", 0, 0);
        playerParent.transform.GetChild(0).GetComponent<Animator>().Play("respawn", 0, 0);
        yield return new WaitForSeconds(2f);
        m_tiempoLimite = tiempoLimite;
        m_vidas--;
        ModificarHudVidas();
        isdead = false;
        if(m_vidas < 1)
        {
            GameOver();
        }
    }

    public void GanarNivel()
    {
        GetComponent<AudioSource>().Play(0);
        VictoryParticlesSystem.GetComponent<ParticleSystem>().Play();
        haswin = true;
        levelWinPanel.SetActive(true);
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }
    public void ReiniciarNivel()
    {
        VictoryParticlesSystem.GetComponent<ParticleSystem>().Stop();
        playerParent.transform.position = new Vector3(0, 0.239f, 0);
        Time.timeScale = 1;
        m_vidas = vidas;
        ModificarHudVidas();
        m_tiempoLimite = tiempoLimite;
        haswin = false;
        isdead = false;
    }

}
