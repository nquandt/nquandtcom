generate_post_data()
{
  cat <<EOF
{
  "routes": [
    "/",
    "/resume",
  ]
}
EOF
}

curl -i \
-H "Accept: application/json" \
-H "Content-Type:application/json" \
--http2 \
-X POST --data "$(generate_post_data)" "https://nquandt.com/update"
