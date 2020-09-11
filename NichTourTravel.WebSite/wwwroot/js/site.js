
function run_waitMe(el, num, text) {
    fontSize = '';
    switch (num) {
        case 1:
            maxSize = '';
            textPos = 'vertical';
            break;
        case 2:
            text = '';
            maxSize = 30;
            textPos = 'vertical';
            break;
        case 3:
            maxSize = 30;
            textPos = 'horizontal';
            fontSize = '18px';
            break;
    }
    el.waitMe({
        effect: 'facebook',
        text: text,
        bg: 'rgba(255,255,255,0.7)',
        color: '#003300',
        maxSize: maxSize,
        source: 'img.svg',
        textPos: textPos,
        fontSize: fontSize,
        onClose: function () { }
    });
}


function showNotification(title, text, type) {
    Swal.fire(
        {
            position: "top-end",
            title:title,
            type: type,
            text: text,
            showConfirmButton: false,
            timer: 3000
        });
}
