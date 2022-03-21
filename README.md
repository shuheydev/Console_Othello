# コンソールオセロ
CPU同士を対戦させる4人対戦オセロです。
IPlayerインターフェースを実装して対戦させます。

実装するのはIPlayer.Placeメソッドで、盤面の状態を受け取って、自分が石を置く場所を返すだけです。

どんな戦略にするかは自由です。

実装したらそのクラスをProgram.cs内の以下の場所でgamManager.Playersに追加してください。4人以上にはしないでください。

```
gameManager.Players.Add(new Random_CPU("スズキ"));
gameManager.Players.Add(new Random_CPU("タカハシ"));
gameManager.Players.Add(new Random_CPU("ヤマダ"));
gameManager.Players.Add(new Random_CPU("サトウ"));
```

# ルール

1. 各プレイヤーはライフを3点持っています。これが0になったらリタイヤです。
2. すでに置かれていたり、盤外に置こうとしたり、ひっくり返す石がないマスに置こうとすると、お手つきとしてライフが-1されます。
3. 今の所、終了条件があなだらけですが、全てのマスに石が置かれたときに一番スコアが高い人が勝ちです。
4. プレイヤーは4人です。多くても少なくてもいけません。4人です。

# 実行画面
https://user-images.githubusercontent.com/43431002/159196918-426567a2-35dd-4bd5-a3f9-9d95c24aeba9.mp4
