using UnityEngine;

public class CreateWindowsScript : MonoBehaviour
{
    [SerializeField] private GameObject windowOffPrefab;
    [SerializeField] private int windowsAmount;

    [SerializeField] private int columns;

    [SerializeField] private float horizontalGap;

    [SerializeField] private float verticalGap;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnWindows();
    }

    private void SpawnWindows()
    {
        for (int i = 0; i < windowsAmount; i++)
        {
            GameObject window = Instantiate(windowOffPrefab, transform);  // Create as child

            float windowWidth = window.GetComponent<SpriteRenderer>().bounds.size.x;
            float windowHeight = window.GetComponent<SpriteRenderer>().bounds.size.y;

            float totalGridWidth = (columns - 1) * horizontalGap;
            float startX = -totalGridWidth / 2;

            float halfBuildingHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
            float startY = halfBuildingHeight - (windowHeight / 2) - 0.25f;
            
            window.transform.localPosition = new Vector3(
                startX + horizontalGap * (i % columns), 
                startY - horizontalGap * (i / columns), 
                -1
            );
        }
    } 
}
