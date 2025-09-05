using UnityEngine;

public class BookViewController : MonoBehaviour
{
    public static BookViewController Instance { get; private set; }

    [Header("Refer�ncias da UI")]
    public GameObject bookViewCanvas; // Arraste o BookView_Canvas aqui
    public Book bookUI; // Arraste o objeto com o script Book.cs aqui

    [Header("Refer�ncias do Jogador")]
    public Player player; // Arraste o seu objeto Player aqui
    // Se voc� usa um script de controle diferente (ex: PlayerController), mude o tipo da vari�vel
    // e desative esse script espec�fico abaixo.

    public bool isBookOpen { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        // L�gica para fechar o livro
        if (isBookOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBook();
        }
    }

    public void OpenBook(BookData data)
    {
        isBookOpen = true;

        // Passa os sprites do item para a UI do livro
        bookUI.bookPages = data.pages;
        bookUI.currentPage = 0; // Garante que o livro sempre abra na primeira p�gina
        bookUI.UpdateSprites(); // Precisaremos criar este m�todo p�blico em Book.cs

        bookViewCanvas.SetActive(true);

        // Pausa o jogador e libera o cursor
        if (player != null) player.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseBook()
    {
        isBookOpen = false;
        bookViewCanvas.SetActive(false);

        // Devolve o controle ao jogador e prende o cursor
        if (player != null) player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}