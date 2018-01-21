using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

	public static Transform[] GetChildren(this Transform transform) {
		Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < children.Length; i++) {
            children[i] = transform.GetChild(i);
        }
		return children;
	}

	public static int IndexOfClosest(this Vector2[] points, Vector2 to) {
		float smallest = Mathf.Infinity;
        int result = -1;

		for (int i = 0; i < points.Length; i++) {
            float dist = Vector2.Distance(to, points[i]);
            if (dist < smallest) {
                smallest = dist;
                result = i;
            }
        }

        return result;
	}

}