
Shader "Hidden/NewImageEffectShader"
{
	//申明所需的属性
	Properties
	{
		//属性名为_MainTex，面板所显示的名称为Texture，2D只属性的类型
		//"white" 属性默认值
		_MainTex("Texture", 2D) = "white" {}
	}
		//一个Shader程序至少有一个SubShader，系统在渲染时会依次调用，直到找到匹配的SubShader，否则使用最后默认指定的Shader
		SubShader
	{
		//Cull Off：关闭阴影剔除 
		//ZWrite ：将像素的深度写入深度缓存中   
		//Always：将当前深度值写到颜色缓冲中 
		Cull Off ZWrite Off ZTest Always

		//渲染通道
		Pass
		{
		//Shader代码段开始，着色器的代码需要定义在CGPROGRAM-ENDCG之间
		CGPROGRAM
		//指定顶点着色器
		#pragma vertex vert
		//指定片元着色器
		#pragma fragment frag
		//引入Unity内置定义
		#include "UnityCG.cginc"

		//定义顶点着色器输入结构体
		struct appdata
		{
		//float4是思维向量，这里相当于告诉渲染引擎，这个属性代表的含义
		float4 vertex : POSITION;
		//纹理
		float2 uv : TEXCOORD0;
	};

	//与上边类似，这里使用一个结构体来定义顶点着色器的输出
	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	//Vertex 顶点函数实现
	v2f vert(appdata v)
	{
		v2f o;
		//传递进来的顶点坐标是模型坐标系中的坐标值，需要经过矩阵转换车成屏幕坐标
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		//将计算后的结果输出给渲染引擎，底层会根据具体的语义去做对应的处理
		return o;
	}

	sampler2D _MainTex;

	//fragment 片元函数实现
	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv);
		col.rgb = 1 - col.rgb;
		return col;
	}
	ENDCG
}
	}
}