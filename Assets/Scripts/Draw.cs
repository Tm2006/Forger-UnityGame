using UnityEngine;
using UnityEngine.InputSystem;

public class Draw : MonoBehaviour
{
    public Camera cam;
    public int totalXpixels = 1024;
    public int totalYpixels = 512;
    public int brushSize = 4;
    public Color BrushColor;

    public bool useInterpolation = true;

    public Transform topLeftCorner;
    public Transform bottomRightCorner;
    public Transform point;

    public Material material;

    public Texture2D generatedTexture;

    Color[] colorMap;

    int xPixel=0;
    int yPixel=0;

    bool pressedLastFrame = false;
    int lastX = 0;
    int lastY = 0;

    float xMult;
    float yMult;

    private void Start()
    {
        colorMap = new Color[totalXpixels * totalYpixels];
        generatedTexture = new Texture2D(totalXpixels, totalYpixels, TextureFormat.RGBA32, false);
        generatedTexture.filterMode = FilterMode.Point;
        material.mainTexture = generatedTexture;
        ResetColor();
        
        xMult = totalXpixels / (bottomRightCorner.localPosition.x - topLeftCorner.localPosition.x); 
        yMult = totalYpixels / (topLeftCorner.localPosition.y - bottomRightCorner.localPosition.y);

        
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            CalculatePixel();
        }
        else
        {
            pressedLastFrame = false;
        }
    }

    void CalculatePixel()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"Hit: {hit.collider.name} | pixel: ({xPixel}, {yPixel})");
            point.position = hit.point;
            xPixel = (int)((point.localPosition.x - topLeftCorner.localPosition.x) * xMult);
            yPixel = (int)((point.localPosition.y - bottomRightCorner.localPosition.y) * yMult);
            ChangePixelsAroundPoint();
        }
        else
        {
            pressedLastFrame = false;
        }
    }

    void ChangePixelsAroundPoint() 
    {
        if(useInterpolation && pressedLastFrame && (lastX != xPixel || lastY != yPixel))
        {
            int dist = (int)Mathf.Sqrt((xPixel - lastX) * (xPixel - lastX) + (yPixel - lastY) * (yPixel - lastY));
            for (int i = 1; i <= dist; i++)
                DrawBrush((i * xPixel + (dist - i) * lastX) / dist, (i * yPixel + (dist - i) * lastY) / dist);
        }
        else
            DrawBrush(xPixel, yPixel);
        pressedLastFrame = true;
        lastX = xPixel;
        lastY = yPixel;
        SetTexture();
    }

    void DrawBrush(int xPix, int yPix)
    { 
        int i = xPix - brushSize + 1, j = yPix - brushSize + 1, maxi = xPix + brushSize - 1, maxj = yPix + brushSize - 1;
        if (i < 0)
            i = 0;
        if (j < 0)
            j = 0;
        if (maxi >= totalXpixels)
            maxi = totalXpixels - 1;
        if (maxj >= totalYpixels)
            maxj = totalYpixels - 1;

        for(int x=i; x<=maxi; x++) {
            for(int y=j; y<=maxj; y++){
                if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize)
                    colorMap[y * totalXpixels + x] = BrushColor;
            }
        }
    }

    void SetTexture()
    {
        generatedTexture.SetPixels(colorMap);
        generatedTexture.Apply();
    }

    void ResetColor()
    {
        for (int i = 0; i < colorMap.Length; i++)
        {
            colorMap[i] = Color.white;
        }
        SetTexture();
    }
}


