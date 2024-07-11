using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Grid
{

        #region Grid
    
    public class Grid
    {
        private float _cellSize;
        private int _width;
        private int _height;
        private int[,] _gridArray;

        public Grid(float cellSize, int width, int height)
        {
            _cellSize = cellSize;
            _width = width;
            _height = height;

            _gridArray = new int[_width, _height];

                #region DebuggingThings

            // for (int x = 0; x < _width; x++)
            // {
            //     for (int y = 0; y < _height; y++)
            //     {
            //         DebugGrid.ShowWorldGrid(_gridArray[x, y].ToString(), null,
            //             GetWorldPosition(x, y) + new Vector3(_cellSize, 0, _cellSize) * 0.5f, 40, Color.white);
            //
            //         Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            //         Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            //     }
            // }
            // Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
            // Debug.DrawLine(GetWorldPosition(0, _width), GetWorldPosition(_width, _height), Color.white, 100f);
            //
                #endregion
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * _cellSize;
        }

        public void TakeValue(Vector3 worldPosition, int value)
        {
            int x, y;
            Vector2 xy = GetXY(worldPosition);
            x = (int) xy.x;
            y = (int) xy.y;
            SetValue(x, y, value);
        }

        private Vector2 GetXY(Vector3 worldPosition)
        {
            Vector2 xy = new Vector2();
            xy.x = Mathf.FloorToInt(worldPosition.x / _cellSize);
            xy.y = Mathf.FloorToInt(worldPosition.z / _cellSize);
            return xy;
        }
        
        public void SetValue(int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
            }
        }
        
        public int SearchValue(Vector3 worldPosition)
        {
            int x, y;
            Vector2 xy = GetXY(worldPosition);
            x = (int) xy.x;
            y = (int) xy.y;
            return GetValue(x, y);
        }
        
        public int GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y];
            }
            return -1;
        }
    }
    
        #endregion

        #region OtherDebugging
    public class DebugGrid
    {
        public static TextMesh ShowWorldGrid(string text, Transform parent = null,
            Vector3 localPosition = default(Vector3), int fontSize = 40, Color color = default(Color),
            TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center,
            int sortingOrder = 0)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
    }
        #endregion
}
