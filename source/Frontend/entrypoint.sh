#!/bin/ash
env | while read -r line; do
  # Get the string before = (the var name)
  name=`echo $line | sed 's/=.*//'`
  eval value="\$$name"
  case "$name" in
    VD_REACT_APP_*)
        eval value="\$$name"
        ESCAPE=$(echo $value | sed -e 's/[]:\/&$*.^[]/\\&/g');#
        echo "Replacing $name with $value ($ESCAPE)"
        $(exec sed -i.bak.html s/\{\{$name\}\}/$ESCAPE/gi /usr/share/nginx/html/index.html)
        # For debugging purposes
        #$(exec sed s/\{\{$name\}\}/$ESCAPE/gi /usr/share/nginx/html/index.html > /usr/share/nginx/html/lol.html)
        ;;
  esac
done

#done <<EOF
#$(env)
#EOF

$(rm /usr/share/nginx/html/index.html.bak.html)

exec $(which nginx) -g "daemon off;"