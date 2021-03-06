import {createMuiTheme} from "@material-ui/core";
import {green, purple} from "@material-ui/core/colors";

export const myTheme = createMuiTheme({
    palette: {
        primary: {
            main: purple[500],
        },
        secondary: {
            main: green[500],
        },
    },
});