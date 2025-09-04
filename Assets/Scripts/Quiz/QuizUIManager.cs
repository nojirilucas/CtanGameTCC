using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class QuizUIManager : MonoBehaviour
{
    public static QuizUIManager Instance { get; set; }

    [Header("Configurações do Quiz")]
    [Tooltip("Quantas perguntas aleatórias serão selecionadas para cada quiz.")]
    public int numeroDePerguntasPorQuiz = 5;

    [Header("UI References")]
    public GameObject parentPanel;
    public TextMeshProUGUI textoDaPergunta;
    public List<Button> botoesDeResposta;

    private Player player;
    private Materia materiaAtual;
    private int indicePerguntaAtual;

    private List<Pergunta> perguntasDaSessao;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        perguntasDaSessao = new List<Pergunta>();
    }

    public void IniciarQuiz(Materia materia, Player interactingPlayer)
    {
        if (parentPanel != null) parentPanel.SetActive(true);

        this.player = interactingPlayer;
        materiaAtual = materia;
        indicePerguntaAtual = 0;

        List<Pergunta> poolDePerguntas = new List<Pergunta>(materia.perguntasDoQuiz);
        perguntasDaSessao.Clear();

        int numeroDePerguntas = Mathf.Min(numeroDePerguntasPorQuiz, poolDePerguntas.Count);

        for (int i = 0; i < numeroDePerguntas; i++)
        {
            int randomIndex = Random.Range(0, poolDePerguntas.Count);
            perguntasDaSessao.Add(poolDePerguntas[randomIndex]);
            poolDePerguntas.RemoveAt(randomIndex);
        }

        if (player != null) player.FreezePlayer(true);

        MostrarPergunta();
    }

    void MostrarPergunta()
    {
        foreach (var botao in botoesDeResposta)
        {
            botao.interactable = true;
        }

        Pergunta pergunta = perguntasDaSessao[indicePerguntaAtual];
        textoDaPergunta.text = pergunta.textoDaPergunta;

        for (int i = 0; i < botoesDeResposta.Count; i++)
        {
            if (i < pergunta.respostas.Length)
            {
                botoesDeResposta[i].gameObject.SetActive(true);
                botoesDeResposta[i].GetComponentInChildren<TextMeshProUGUI>().text = pergunta.respostas[i];
                int indiceResposta = i;
                botoesDeResposta[i].onClick.RemoveAllListeners();
                botoesDeResposta[i].onClick.AddListener(() => OnRespostaSelecionada(indiceResposta));
            }
            else
            {
                botoesDeResposta[i].gameObject.SetActive(false);
            }
        }
    }

    void OnRespostaSelecionada(int indiceSelecionado)
    {
        Pergunta pergunta = perguntasDaSessao[indicePerguntaAtual];
        if (indiceSelecionado == pergunta.indiceRespostaCorreta)
        {
            Debug.Log("Resposta Correta!");
            ProximaPergunta();
        }
        else
        {
            Debug.Log("Resposta Errada!");
            StartCoroutine(HandleWrongAnswer());
        }
    }

    void ProximaPergunta()
    {
        indicePerguntaAtual++;
        if (indicePerguntaAtual < perguntasDaSessao.Count)
        {
            MostrarPergunta();
        }
        else
        {
            Debug.Log("Quiz finalizado com sucesso!");
            if (parentPanel != null) parentPanel.SetActive(false);
            if (!materiaAtual.foiConcluida)
            {
                GameManager.Instance.ConcluirMateria(materiaAtual.nomeDoQuiz);
            }
            if (player != null) player.UnfreezePlayer();
        }
    }

    private IEnumerator HandleWrongAnswer()
    {
        textoDaPergunta.text = "Resposta Errada!";

        foreach (var botao in botoesDeResposta)
        {
            botao.interactable = false;
        }

        yield return new WaitForSeconds(2f);

        if (parentPanel != null) parentPanel.SetActive(false);
        if (player != null) player.UnfreezePlayer();
    }
}