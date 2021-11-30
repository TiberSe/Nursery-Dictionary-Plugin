# Nursery-Dictionary-Plugin

Nursery Dictionary Pluginは、[Nursery](https://github.com/noonworks/Nursery)(MIT Lincense ©2018 noonworks)に辞書機能を追加するプラグインです。
現在日本語のみに対応しています。

<br>
<br>

# 導入方法
Releaseで配布されているDictPlugin.zipの中身を丸ごとNursery.exeがあるフォルダに突っ込んだ後、[Nursery.exe]/plugins/plugins.jsonに下記のロードオーダーを追加してください

```json
    "Septim.DictPlugin.Dictionary",
    "Septim.DictPlugin.DictionaryCommand"
```
<br>
<br>

# 使用方法
起動時に自動で有効化され、読み上げ内容に指定文字列があった場合に自動で置き換えます。

<br>
<br>

# コマンド一覧

> ## 新しく辞書に追加する
> ``` $learn args1 args2 ```
> 
> args1の読みをarsg2として登録する

<br>
  
> ## 辞書から削除する
> ``` $forget args1 ```
> 
> args1の読みを削除する

<br>

> ## 辞書登録されている単語すべてを表示する
> ``` $list ```
> 

<br>

>  ## 辞書機能を一時的に無効化する
> ``` $unableDictionary ```
> 

<br>

>  ## 辞書機能の無効化を解除する。
> ``` $enableDictionary ```
> 
