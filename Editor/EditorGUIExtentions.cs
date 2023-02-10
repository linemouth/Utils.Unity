using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public static class EditorGUIExtentions
{
    private struct SegmentInfo
    {
        public float fixedSize;
        public float growWeight;
        public float shrinkWeight;

        public SegmentInfo(float size, float grow, float shrink)
        {
            fixedSize = size;
            growWeight = grow;
            shrinkWeight = shrink;
        }
    }

    public static Rect[] GetRowRects(Rect contentRect, string rowDescription, float gap = 0)
    {
        List<SegmentInfo> segments = Regex.Matches(rowDescription, @"(?=[\d\.\+-])(?<size>[\d\.]+)?(?:\+(?<grow>[\d\.]+))?(?:-(?<shrink>[\d\.]+))?")
            .Select(match => new SegmentInfo(
                match.Groups["size"].Success ? float.Parse(match.Groups["size"].Value) : 0,
                match.Groups["grow"].Success ? float.Parse(match.Groups["grow"].Value) : 0,
                match.Groups["shrink"].Success ? float.Parse(match.Groups["shrink"].Value) : 0
            )).ToList();
        float fixedWidth = segments.Sum(seg => seg.fixedSize);
        float availableWidth = contentRect.width - gap * (segments.Count - 1) - fixedWidth;
        float fractionalTotal = availableWidth >= 0 ? segments.Sum(s => s.growWeight) : segments.Sum(s => s.shrinkWeight);
        float fractionalScale = availableWidth / fractionalTotal;
        float x = contentRect.x;
        List<Rect> rects = new List<Rect>();
        foreach(var segment in segments)
        {
            float width = segment.fixedSize + fractionalScale * (fractionalScale > 0 ? segment.growWeight : segment.shrinkWeight);
            rects.Add(new Rect(x, contentRect.y, width, contentRect.height));
            x += width + gap;
        }
        return rects.ToArray();
    }
}
