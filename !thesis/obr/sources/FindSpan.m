function [i] = FindSpan(p,u,U)
%FINDSPAN2 Summary of this function goes here
%   Detailed explanation goes here
if u==U(length(U))
    i=length(U)-1-p-1;
    return
end
i=0;
while i<length(U) && u>=U(i+1+1)
    i=i+1;
end
end

