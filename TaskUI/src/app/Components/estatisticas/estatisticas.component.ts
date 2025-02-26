import { Component, OnInit, AfterViewInit } from '@angular/core';
import { StatisticsService } from '../../../Services/statistics.service';
import Chart from 'chart.js/auto';
import { AuthService } from '../../../Services/auth.service';

@Component({
  selector: 'app-estatisticas',
  templateUrl: './estatisticas.component.html',
  styleUrls: ['./estatisticas.component.css']
})
export class EstatisticasComponent implements OnInit, AfterViewInit {
  public statistics: any = {
    userCount: 0,
    totalTasks: 0,
    publicTasks: 0,
    privateTasks: 0
  };
  public unique_name: string = ''; 
  private myChart: any; 
  constructor(private statisticsService: StatisticsService, private auth: AuthService) { }

  ngOnInit(): void {
    this.loadStatistics();
    this.unique_name = localStorage.getItem('unique_name') || ''; 
  }

  ngAfterViewInit(): void {
    this.createChart(); 
  }

  loadStatistics(): void {
    this.statisticsService.getStatistics().subscribe(
      data => {
        this.statistics = data;
        this.createChart(); 
      },
      error => {
        console.error('Erro ao carregar estatísticas', error);
      }
    );
  }

  createChart(): void {
    const ctx = document.getElementById('myChart') as HTMLCanvasElement;
    if (this.myChart) {
      this.myChart.destroy(); 
    }
    this.myChart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['Tarefas Públicas', 'Tarefas Privadas'],
        datasets: [{
          label: 'Distribuição de Tarefas',
          data: [this.statistics.publicTasks, this.statistics.privateTasks], 
          backgroundColor: [
            '#16a085', 
            '#e74c3c'  
          ],
          borderColor: '#fff',
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: 'Distribuição de Tarefas'
          }
        }
      }
    });
  }

  logout() {
    this.auth.logout();
  }
}