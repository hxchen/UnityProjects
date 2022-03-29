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

7. 增加新的瓦片类型Lock

   1. 添加图片资源并设置相关属性(单位像素)

   2. 在Breakable Tile基础之上重新制作瓦片,设置Sprite.

   3. 设置Order in Layer

   4. 制作成预制件

   5. 脚本文件`Board.cs`中`TileKind`增加新的类型`Lock` ，

      `[Header("Prefabs")]`声明增加

      ```c#
      public GameObject lockTilePrefab;
      ```

      `[Header("Layout")]`声明增加

      ```c#
      public BackgroundTile[,] lockTiles;

   6. 初始化新类型瓦片

      ```c#
      public void GenerateLockTiles() {
              for (int i = 0; i < boardLayout.Length; i++) {
                  if (boardLayout[i].tileKind == TileKind.Lock) {
                      Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                      GameObject tile = Instantiate(lockTilePrefab, tempPosition, Quaternion.identity);
                      lockTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
                  }
              }
          }
      ```

   7. 编辑Level模板，Board Layout 添加新的点位为新类型

   8. 编辑Board，Prefabs关联Lock Tile Prefab

   9. `Board.cs Start` 方法增加初始化

      ```c#
      lockTiles = new BackgroundTile[width, height];
      ```

   10. `Board.cs`  增加初始化调用

       ```c#
       GenerateLockTiles();
       ```

   11. 运行Unity查看

   12. `Dot.cs`功能实现，`MovePiecesActual`增加移动检查

   13. `Board.cs`功能实现，`DestroyMatchesAt` 增加`Lock`检查

