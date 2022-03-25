1. ###### 制作预制件

2. ###### 游戏对象和脚本

3. ###### 特效预制件制作

   1. 创建Material对象，添加图像，Shader应用Legacy Shaders/Particles/Alpha Blended，设置相应Tint color。
   2. 创建特效系统，添加Material，设置相关参数。
   3. 特效系统制作成预制件。

4. 动态调整显示大小

   1. Camera Position
   2. Orthographic size （camera pos = frame/2）
   3. Aspect Ratio

5. 动作创作

   1. 创建Animator（区别于Animation包含状态机，可以多个动画转换；Animation单个动画）组件。
   2. 创建Animator Controller并绑定Animator。
   3. windows打开Animator和Animation。
   4. Animation窗体中创建Animation。

6. Animation制作

   1. 选中GameObject上,create new clip（可以复制使用）

   2. 可以录制XYZ等变化

   3. make transition

   4. parameters（例如设置bool型名为Shine的参数）

   5. 设置transition属性Conditions（例如设置Shine = true）

   6. 定义以及使用

      ```C#
      // 声明
      Animator animator;
      // 实现
      animator = GetComponent<Animator>();
      //使用
      animator.SetBool("Shine", true);
      ```

      

      

   

7. 

8. 

