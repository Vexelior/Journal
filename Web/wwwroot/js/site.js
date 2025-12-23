document.addEventListener('DOMContentLoaded', function () {
    $('#summernote').summernote({
        height: 300,
        placeholder: 'Write your journal entry here...',
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'underline', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link']],
            ['view', ['fullscreen', 'codeview', 'help']]
        ]
    });
    document.querySelectorAll('.dropdown-submenu > .dropdown-toggle').forEach(function (element) {
        element.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            var submenu = this.nextElementSibling;
            if (submenu) {
                submenu.classList.toggle('show');
            }
        });
    });
    if (window.matchMedia('(min-width: 768px)').matches) {
        document.querySelectorAll('.dropdown-submenu').forEach(function (element) {
            element.addEventListener('mouseenter', function () {
                var submenu = this.querySelector('.dropdown-menu');
                if (submenu) {
                    submenu.classList.add('show');
                }
            });
            element.addEventListener('mouseleave', function () {
                var submenu = this.querySelector('.dropdown-menu');
                if (submenu) {
                    submenu.classList.remove('show');
                }
            });
        });
        document.querySelectorAll('.dropdown').forEach(function (element) {
            element.addEventListener('mouseenter', function () {
                var menu = this.querySelector('.dropdown-menu');
                if (menu) {
                    menu.classList.add('show');
                }
            });

            element.addEventListener('mouseleave', function () {
                var menu = this.querySelector('.dropdown-menu');
                if (menu) {
                    menu.classList.remove('show');
                }
            });
        });
    }
});