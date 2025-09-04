using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public List<Materia> todasAsMaterias;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        InicializarMaterias();
    }

    void InicializarMaterias()
    {
        foreach (var materia in todasAsMaterias)
        {
            materia.foiConcluida = false;

            if (materia.preRequisitos == null || materia.preRequisitos.Count == 0)
            {
                materia.estaDisponivel = true;
            }
            else
            {
                materia.estaDisponivel = false;
            }
        }
    }

    public void ConcluirMateria(string nomeDoQuiz)
    {
        Materia materiaConcluida = todasAsMaterias.Find(m => m.nomeDoQuiz == nomeDoQuiz);

        if (materiaConcluida != null && !materiaConcluida.foiConcluida)
        {
            materiaConcluida.foiConcluida = true;
            Debug.Log("Matéria concluída: " + materiaConcluida.nomeDoQuiz);

            AtualizarMateriasDisponiveis();
        }
    }

    void AtualizarMateriasDisponiveis()
    {
        foreach (var materia in todasAsMaterias)
        {
            if (materia.estaDisponivel || materia.foiConcluida) continue;

            bool todosRequisitosCumpridos = true;
            foreach (var requisitoId in materia.preRequisitos)
            {
                Materia materiaRequisito = todasAsMaterias.Find(m => m.nomeDoQuiz == requisitoId);

                if (materiaRequisito == null || !materiaRequisito.foiConcluida)
                {
                    todosRequisitosCumpridos = false;
                    break;
                }
            }

            if (todosRequisitosCumpridos)
            {
                materia.estaDisponivel = true;
                Debug.Log("Nova matéria desbloqueada: " + materia.nomeDoQuiz);
            }
        }
    }
}