$(function () {
    createCategoryChart();
    createAuthorChart();
});


function createCategoryChart() {
    $.ajax({
        url: "/Reports/GetFirstChart"
    }).done(function (data) {
        console.log(data)
        var categories = [];
        var sums = [];
        var colors = [];
        for (var i = 0; i < data.length; i++) {
            categories.push(data[i].categoryName);
            sums.push(data[i].sumCategory);
            colors.push('rgba(' + Math.floor(Math.random() * 256) + ',' + Math.floor(Math.random() * 256) + ',' + Math.floor(Math.random() * 256) + ', 0.9)');
        }
        var catCanvas = document.getElementById('chartCategory');
        var catChart = new Chart(catCanvas, {
            type: 'pie',

            data: {
                labels: categories,
                datasets: [{
                    label: '# of books',
                    data: sums,
                    hoverOffset: 4,
                    backgroundColor: colors
                }],
            },
            options: {
                responsive: true,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
            },
        });
    });
}


function createAuthorChart() {
    $.ajax({
        url: "/Reports/GetSecondChart"
    }).done(function (data) {
        console.log(data)
        var authors = [];
        var sums = [];
        var colors = [];
        for (var i = 0; i < data.length; i++) {
            authors.push(data[i].authorName);
            sums.push(data[i].countBook);
            colors.push('rgba(' + Math.floor(Math.random() * 256) + ',' + Math.floor(Math.random() * 256) + ',' + Math.floor(Math.random() * 256) + ', 0.9)');
        }
        var authorCanvas = document.getElementById('chartAuthor');
        var authorChart = new Chart(authorCanvas, {
            type: 'pie',
            data: {
                labels: authors,
                datasets: [{
                    label: '# of books',
                    data: sums,
                    hoverOffset: 4,
                    backgroundColor: colors
                }]
            },
            options: {
                responsive: true,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
            },
        });
    });
}