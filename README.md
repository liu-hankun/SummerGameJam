# SummerGameJam
- 7.28 Hankun
- 主要就是加了一些动画和一个game manager，里面主要是一些UI的关闭和暂停面板以及游戏结束面板，其中可以调整游戏持续时间
然后加了start scene。剩下的功能就看你了


- 7.20 Hankun
- 我这边新添加了6个脚本，都是关于食材区域的。
- -首先最重要的脚本是EnemyManager，这个脚本跟EnemyMovement相关联，里面包含了除了冰块以外所有的怪物的功能，移动，血量，速度，掉落物。你直接把EnemyManager绑定给一个空的game object就可以实现功能。然后你就可以添加想要的信息在里。在Test Scene里面我挂载到同名（EnemyManager）的文件下面的了。Spawn Area就是带有collider2D的食材移动区域
- 然后就是IceBlock这个代码，因为冰块的功能有些不一样我就单独拿出来写了，在Test Scene中脚本是挂载到Ice Monster上的，具体功能就是玩家靠近冰块右键就可以获取冰块了
- 然后是Bullet和BulletShooter这两个脚本，都是player射击的功能的。BulletShooter直接绑定在Test-player上面的。子弹需要带有tag“bullet”
- 最后是InventoryManager，这个代码你厨房区域应该会用到，这个是玩家仓库的代码，就是玩家收集的食材冰块的数据都会保存显示在里面
- 然后你run游戏走到食材区域就可以点按左键射击食材，右键到冰块附近去打冰块了，然后收集的物品信息会显示在右下角。



7. 26 lzy

1. 限制玩家只能持有一个食材
在 InventoryManager 中添加了判断逻辑：

玩家如果已持有物品，则无法再次从冰箱取出食材。

成功拿取后立即关闭冰箱界面。

修改了冰箱 FridgeTool.cs 和 FridgeUIManager.cs，增加了“关闭面板”和“交互限制”的逻辑。

2. 完成配方后给予得分
创建了 OrderManager.cs，用于生成任务订单和处理配方完成逻辑；

调用 CompleteOrder() 方法完成配方验证；

正确提交后分数 +10，错误提交无反应。

3. 每次任务限时 60 秒
在 OrderManager.cs 中添加计时器 timer 和倒计时逻辑；

超过时间自动生成下一个随机任务；

Update() 中持续递减时间并更新 UI。

4. 任务、分数、倒计时 UI 实时显示
修改 UIOrderPanel.cs：

增加 timerText 变量；

新增方法 UpdateTimerDisplay() 显示倒计时；

在 Canvas 中添加了 TextMeshProUGUI 元素并绑定至 UIOrderPanel；

UI 实时显示任务名称、当前分数、剩余时间。

5. 将配方名从中文统一改为英文，解决乱码
RecipeBook.asset 中将所有 recipeName 替换为英文（如 "Mango Smoothie"）；

所有 ingredient（原料名）也改为英文以保持一致性；

避免乱码和中文字体兼容问题，适配 TextMeshPro 默认字体。

6. 取消菜板切菜功能（简化流程）
对 CuttingBoardTool.cs 进行删减：

去除“切芒果/葡萄”的逻辑；

可能直接使用从冰箱中取出的果肉作为可用原料。

7. 支持在组装台按 Q 键合成冰沙
在 player.cs 中添加了检测玩家手持食材与配方是否匹配的功能；

满足条件时可按 Q 进行合成；

合成成功后调用 OrderManager.Instance.CompleteOrder()；

重置任务并增加分数。

要完成的内容：
	1 完成水果的prefab 并绑定到FridgeUIPanel中（在UI里找）
	2 需要你完成一下倒计时结束游戏
	3需要测试一下组织功能，因为我没有绑prefab， 所有暂时用不了



7. 30 lzy

	e键打开冰箱
	q键提交物品
	space组合材料