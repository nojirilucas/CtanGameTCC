using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FluxogramaManager : MonoBehaviour
{
    public static FluxogramaManager Instance { get; private set; }

    [System.Serializable]
    public class MateriaUI
    {
        public string nomeDoQuiz;
        public Button botao;
        // Alterado de 'borda' para 'iconeDeEstado' para ficar mais claro
        public Image iconeDeEstado;
    }

    [Header("Configura��o das Mat�rias")]
    public List<MateriaUI> materiasNoFluxograma;

    // Alterado de Cores para Sprites
    [Header("�cones de Estado")]
    public Sprite iconeDisponivel; // Ex: c�rculo verde
    public Sprite iconeBloqueado;  // Ex: c�rculo vermelho
    public Sprite iconeConcluido;  // Ex: c�rculo amarelo/azul

    private Player playerAtual;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void OnEnable()
    {
        AtualizarVisualizacao();
    }

    public void AtualizarVisualizacao()
    {
        foreach (var materiaUI in materiasNoFluxograma)
        {
            if (materiaUI.iconeDeEstado == null) continue; // Pula se o �cone n�o estiver configurado

            Materia materiaData = GameManager.Instance.todasAsMaterias.Find(m => m.nomeDoQuiz == materiaUI.nomeDoQuiz);

            if (materiaData == null) continue;

            if (materiaData.foiConcluida)
            {
                // Altera o sprite em vez da cor
                materiaUI.iconeDeEstado.sprite = iconeConcluido;
                materiaUI.botao.interactable = true;
            }
            else if (materiaData.estaDisponivel)
            {
                materiaUI.iconeDeEstado.sprite = iconeDisponivel;
                materiaUI.botao.interactable = true;
            }
            else
            {
                materiaUI.iconeDeEstado.sprite = iconeBloqueado;
                materiaUI.botao.interactable = false;
            }
        }
    }

    public void OnMateriaSelecionada(string nomeDoQuiz)
    {
        Materia materiaData = GameManager.Instance.todasAsMaterias.Find(m => m.nomeDoQuiz == nomeDoQuiz);
        if (materiaData == null) return;

        if (playerAtual == null) playerAtual = FindObjectOfType<Player>();

        QuizUIManager.Instance.IniciarQuiz(materiaData, playerAtual);

        gameObject.SetActive(false);
    }

    public void FecharFluxograma()
    {
        if (playerAtual == null) playerAtual = FindObjectOfType<Player>();

        if (playerAtual != null)
        {
            playerAtual.UnfreezePlayer();
        }
        gameObject.SetActive(false);
    }
}