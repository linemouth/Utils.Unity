
float GetViewDepth(float4 position) {
	float4 screenPosition = UnityObjectToClipPos(input.position);
	float4 clipPos = screenPos / screenPos.w;

}






