clc;clear;close;
%% VSTUP
p=1;
U=[0,0,0,1/4,2/4,3/4,3/4,1,1,1];

%%
figure; hold on;
%xlabel([num2str(U(1)) ' <= u <= ' num2str(U(length(U)))]);
xlabel('u');
ylabel('N_{i,p}');

iter=U(length(U))*1000;
for k=0:iter
    u=k/1000;
    i=FindSpan(p,u,U);
    N=BasisFuns(i,u,p,U);
    m=length(U)-1;
    n=m-p-1;
if u==U(m+1)
%    Nip(k+1,:)=zeros(k+1,n);
    Nip(k+1,n)=1;
end
         for j=1:(p+1)
             Nip(k+1,i-p+j)=N(j);
         end
%     Nip(k+1,i-2+1)=N(1);
%     Nip(k+1,i-1+1)=N(2);
%     Nip(k+1,i+1)=N(3);
end
for k=1:size(Nip,2)
    Y=Nip(:,k)';
    Y(Y==0)=NaN;
    X=0:0.001:U(length(U));
    plot(X,Y);
    legendInfo{k}=['N_{' num2str(k-1) ',' num2str(p) '}'];
end

legend(legendInfo,'Location', 'bestoutside');
tit=['p = ' num2str(p) '; U = \{'];
for k=1:length(U)
    tit=[tit num2str(U(k))];
    
    if k~=length(U)
        tit=[tit '; '];
    end
end
tit=[tit '\}'];
%title(tit);
ylim([0 1.1])
yticks(0:0.1:1)

print -depsc graf