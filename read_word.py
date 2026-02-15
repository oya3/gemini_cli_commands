# read_word.py
import sys
import base64

def main():
    if len(sys.argv) < 2:
        return
    
    file_path = sys.argv[1]
    try:
        with open(file_path, "rb") as f:
            # WordバイナリをBase64文字列に変換して標準出力に書き出す
            encoded = base64.b64encode(f.read()).decode('utf-8')
            print(encoded)
    except FileNotFoundError:
        # エラーメッセージを返すとGemini側で「わかりません」等の判断材料になります
        print(f"Error: {file_path} が見つかりません。")

if __name__ == "__main__":
    main()
