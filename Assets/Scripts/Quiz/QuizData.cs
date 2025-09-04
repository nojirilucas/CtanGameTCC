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

    [Header("Estado da Matéria")]
    public bool foiConcluida = false;
    public bool estaDisponivel = false;

    [Header("Relações de Pré-requisito")]
    public List<string> preRequisitos;

    [Header("Conteúdo do Quiz")]
    public Pergunta[] perguntasDoQuiz;
}