using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pergunta
{
    [TextArea(3, 10)]
    public string textoDaPergunta;
    public string[] respostas;
    public int indiceRespostaCorreta;
}

[System.Serializable]
public class Materia
{
    public string nomeDoQuiz;

    [Header("Estado da Mat�ria")]
    public bool foiConcluida = false;
    public bool estaDisponivel = false;

    [Header("Rela��es de Pr�-requisito")]
    public List<string> preRequisitos;

    [Header("Conte�do do Quiz")]
    public Pergunta[] perguntasDoQuiz;
}