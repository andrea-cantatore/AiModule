using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Grid
{
    public class GridTesting : MonoBehaviour
    {
        [SerializeField] private float _cellSize;
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        private void Awake()
        {
            Grid grid = new Grid(_cellSize, _width, _height);
        }
    }
}
