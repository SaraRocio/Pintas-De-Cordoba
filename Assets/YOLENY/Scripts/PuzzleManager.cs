using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
   public static PuzzleManager Instance;
    [Header("Cantidad de piezas")] public int totalPiezas = 4; 
    private int piezasColocadas = 0; private bool puzzleCompletado = false; 
    void Awake() { Instance = this; } public void PiezaColocada() { piezasColocadas++; 
        Debug.Log("Piezas colocadas: " + piezasColocadas + "/" + totalPiezas); 
        if (piezasColocadas >= totalPiezas) { CompletarPuzzle(); } } 
    void CompletarPuzzle() { if (puzzleCompletado) return;
        puzzleCompletado = true; Debug.Log("¡PUZZLE COMPLETADO!"); } }